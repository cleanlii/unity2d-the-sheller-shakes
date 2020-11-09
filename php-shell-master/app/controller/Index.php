<?php
declare (strict_types=1);

namespace app\controller;

use app\BaseController;
use app\Request;
use app\validate\Shell;
use think\db\exception\DataNotFoundException;
use think\db\Mongo;
use think\exception\ValidateException;
use think\facade\Db;

class Index extends BaseController
{
    private static array $visibleKeys = [
        'id', 'transform', 'prefab', 'scene', 'text', 'time'
    ];

    private static function filterData(array $data) {
        if (array_key_exists('_id', $data)) {
            $data['id'] = (string)$data['_id'];
        }
        return array_filter($data, function ($key) {
            return in_array($key, self::$visibleKeys);
        }, ARRAY_FILTER_USE_KEY);
    }


    /**
     * @return Mongo
     */
    private static function getMongo() {
        /** @var Mongo $con */
        $con = Db::name('mongo');
        return $con;
    }

    public function hello() {
        return api(200, null, "Hello");
    }

    public function query(string $scene, int $count) {
        $data = self::getMongo()->cmd([
            'aggregate' => 'shells',
            'allowDiskUse' => true,
            'pipeline' => [
                ['$match' => (object)['scene' => $scene]],
                ['$sample' => ["size" => $count]],
            ],
            'cursor' => new \stdClass,
        ]);
        $data = array_map([self::class, "filterData"], $data);
        return api(200, $data);
    }

    public function pick(string $id) {
        try {
            self::getMongo()->table('shells')->findOrFail($id);
        } catch (DataNotFoundException $e) {
            return api(404, null, 'ID is not exist');
        }
        return api(200, null, 'ok');
    }

    public function create(Request $request) {
        if (!$request->has('shell')) {
            return api(400, null, '参数缺失');
        }
        $shell = $request->post('shell');
        if (is_string($shell)) {
            try {
                $shell = json_decode($shell, true);
            } catch (\JsonException $e) {
                return api(400, null, '参数异常');
            }
        } else if (!is_array($shell)) {
            return api(400, null, '参数异常');
        }
        try {
            $result = validate(Shell::class)->check($shell);
            if ($result !== true) {
                return api(400, null, $result);
            }
        } catch (ValidateException $e) {
            return api(400, null, $e->getError());
        }
        $getV3 = function ($v3) {
            return ['x' => (float)$v3['x'], 'y' => (float)$v3['y'], 'z' => (float)$v3['z']];
        };
        $shell = self::filterData($shell);
        foreach (['position', 'rotation', 'scale'] as $key) {
            $shell['transform'][$key] = $getV3($shell['transform'][$key]);
        }
        /** @var string $id */
        $id = self::getMongo()->table('shells')->insertGetId($shell);
        $shell['id'] = $id;
        return api(200, $shell);
    }
}

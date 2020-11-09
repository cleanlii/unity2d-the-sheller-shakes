<?php
declare (strict_types=1);

namespace app\validate;

use think\Validate;

class Shell extends Validate
{
    protected $rule = [
        'prefab' => ['require', 'alphaDash'],
        'scene' => ['require', 'alphaDash'],
        'nick' => ['require', 'checkString'],
        'text' => ['require', 'checkString'],
        'time' => ['require', 'alphaDash'],
        'transform' => ['require', 'array'],
        'transform.position' => ['require', 'array'],
        'transform.position.x' => ['require', 'float'],
        'transform.position.y' => ['require', 'float'],
        'transform.position.z' => ['require', 'float'],
        'transform.rotation' => ['require', 'array'],
        'transform.rotation.x' => ['require', 'float'],
        'transform.rotation.y' => ['require', 'float'],
        'transform.rotation.z' => ['require', 'float'],
        'transform.rotation.w' => ['require', 'float'],
        'transform.scale' => ['require', 'array'],
        'transform.scale.x' => ['require', 'float'],
        'transform.scale.y' => ['require', 'float'],
        'transform.scale.z' => ['require', 'float'],
    ];

    protected $message = [
        'text.print' => '请输入正常的文本内容',
    ];

    protected function checkString($value) {
        return is_string($value);
    }
}

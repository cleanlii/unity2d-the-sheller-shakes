<?php
// +----------------------------------------------------------------------
// | ThinkPHP [ WE CAN DO IT JUST THINK ]
// +----------------------------------------------------------------------
// | Copyright (c) 2006~2018 http://thinkphp.cn All rights reserved.
// +----------------------------------------------------------------------
// | Licensed ( http://www.apache.org/licenses/LICENSE-2.0 )
// +----------------------------------------------------------------------
// | Author: liu21st <liu21st@gmail.com>
// +----------------------------------------------------------------------
use think\facade\Route;

Route::get('hello', 'index/hello');
Route::get('shell/query/:scene/:count', 'index/query');
Route::get('shell/pick/:id', 'index/pick');
Route::post('shell/create', 'index/create');
Route::miss(function (){
    return api(404, null, 'route not found');
});
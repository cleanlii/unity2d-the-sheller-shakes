<?php
// 应用公共文件

function api($code, $data = null, $msg = '') {
    return json([
        'code' => $code,
        'data' => $data,
        'msg' => $msg,
    ]);
}
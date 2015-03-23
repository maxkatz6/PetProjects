<?php
/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

ini_set('display_errors',1);
error_reporting(E_ALL);


// Path to the chat directory:
define('SCHAT_PATH', dirname($_SERVER['SCRIPT_FILENAME']).'/');
define('VER', 'v4');

require(SCHAT_PATH.'lib/classes.php');

if (Config::$gzipEnabled && substr_count($_SERVER['HTTP_ACCEPT_ENCODING'], 'gzip')) {
   ob_start('ob_gzhandler'); //Сжимаем
}
else if (Config::$debug) {
    ob_start();
}
 
//$firephp->log('');
$sChat = new CustomSChat();

if (Config::$debug) {
    $xhprof_data = xhprof_disable();
    $xhprof_runs = new XHProfRuns_Default();
    $run_id = $xhprof_runs->save_run($xhprof_data, "xhprof_testing");
    // Формируем ссылку на данные профайлинга и записываем ее в консоль
    $link = "http://" . $_SERVER['HTTP_HOST'] . "/schat/chat/debug/xhprof-0.9.4/xhprof_html/index.php?run={$run_id}&source=xhprof_testing\n";
    $firephp = FirePHP::getInstance(true);
    $firephp->info($link);
}
?>
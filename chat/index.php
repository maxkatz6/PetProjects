<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

ini_set('display_errors',1);
error_reporting(E_ALL);


// Path to the chat directory:
define('SCHAT_PATH', dirname($_SERVER['SCRIPT_FILENAME']).'/');
define('VER', 'v9');

require(SCHAT_PATH.'lib/classes.php');

if (Config::$gzipEnabled && substr_count($_SERVER['HTTP_ACCEPT_ENCODING'], 'gzip')) {
   ob_start('ob_gzhandler'); //Сжимаем
}
else if (Config::debug) {
    ob_start();
}

$sChat = new JoomlaSChat();

if (Config::debug && function_exists('xhprof_disable')) {
    $xhprof_data = xhprof_disable();
    $xhprof_runs = new XHProfRuns_Default();
    $firephp = FirePHP::getInstance(true);
    $firephp->info("http://" . $_SERVER['HTTP_HOST'] . "/schat/chat/debug/xhprof-0.9.4/xhprof_html/index.php?run=".$xhprof_runs->save_run($xhprof_data, "xhprof_testing")."&source=xhprof_testing\n");
}
?>
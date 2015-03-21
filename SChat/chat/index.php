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
else {
    ob_start();
}

$sChat = new CustomSChat();
?>
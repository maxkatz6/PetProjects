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

/*if (substr_count($_SERVER['HTTP_ACCEPT_ENCODING'], 'gzip')) {
   ob_start('ob_gzhandler'); //Сжимаем
}
else {
    ob_start();
}*/

// Path to the chat directory:
define('AJAX_CHAT_PATH', dirname($_SERVER['SCRIPT_FILENAME']).'/');

// Include settings and class libraries:
require(AJAX_CHAT_PATH.'lib/classes.php');

define('VER', 'v3');

// Initialize the chat:
$ajaxChat = new CustomAJAXChat($AJAXChatConfig);
?>
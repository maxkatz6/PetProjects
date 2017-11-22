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

define('DS', DIRECTORY_SEPARATOR );
define('SCHAT_PATH', __DIR__);
define('VER', 'v15');

require(SCHAT_PATH.DS.'lib'.DS.'classes.php');

if (Config::$gzipEnabled && substr_count($_SERVER['HTTP_ACCEPT_ENCODING'], 'gzip')) {
   ob_start('ob_gzhandler'); //Сжимаем
}
else if (Config::debug) {
    ob_start();
}

$sChat = new JoomlaSChat();

?>
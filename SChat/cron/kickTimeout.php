<?php

ini_set('display_errors',1);
error_reporting(E_ALL);

define('DS', DIRECTORY_SEPARATOR );
define('SCHAT_PATH', dirname(__DIR__));
define('VER', 'cron');

require(SCHAT_PATH.DS.'lib'.DS.'classes.php');

$sChat = new JoomlaSChat(true);
$sChat->removeInactive();
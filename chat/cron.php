<?php

ini_set('display_errors',1);
error_reporting(E_ALL);

define('SCHAT_PATH', dirname($_SERVER['SCRIPT_FILENAME']).'/');
define('VER', 'v12');

require(SCHAT_PATH.'lib/classes.php');

$sChat = new JoomlaSChat(true);
$sChat->removeInactive(false);
<?php
/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */
// Load configuration
require_once(SCHAT_PATH.'lib/config.php');

// Include Class libraries:
require(SCHAT_PATH.'lib/class/SChat.php');
require(SCHAT_PATH.'lib/class/SChatMySQLiDataBase.php');
require(SCHAT_PATH.'lib/class/SChatMySQLiQuery.php');
require(SCHAT_PATH.'lib/class/SChatEncoding.php');
require(SCHAT_PATH.'lib/class/SChatString.php');
require(SCHAT_PATH.'lib/class/SChatFileSystem.php');
require(SCHAT_PATH.'lib/class/SChatHTTPHeader.php');
require(SCHAT_PATH.'lib/class/SChatLanguage.php');
require(SCHAT_PATH.'lib/class/SChatTemplate.php');
require(SCHAT_PATH.'lib/class/CustomSChat.php');

define( '_JEXEC', 1 );

define( 'DS', DIRECTORY_SEPARATOR );
define('JPATH_BASE', dirname(SCHAT_PATH));

require_once( JPATH_BASE .DS.'includes'.DS.'defines.php' );
require_once( JPATH_BASE .DS.'includes'.DS.'framework.php' );
require_once( JPATH_BASE .DS.'libraries'.DS.'joomla'.DS.'factory.php');

$mainframe = JFactory::getApplication('site');
$mainframe->initialise();

define('J_PREFIX', $mainframe->getCfg('dbprefix'));
Config::$chatClosed  = Config::$chatClosed  || ($mainframe->getCfg('offline') == 1);
Config::$gzipEnabled = Config::$gzipEnabled || $mainframe->getCfg('qzip');

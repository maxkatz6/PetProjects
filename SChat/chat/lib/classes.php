<?php
/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Load configuration
require_once(AJAX_CHAT_PATH.'lib/config.php');

// Include Class libraries:
require(AJAX_CHAT_PATH.'lib/class/AJAXChat.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatDataBase.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatMySQLDataBase.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatMySQLQuery.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatMySQLiDataBase.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatMySQLiQuery.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatEncoding.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatString.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatFileSystem.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatHTTPHeader.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatLanguage.php');
require(AJAX_CHAT_PATH.'lib/class/AJAXChatTemplate.php');
require(AJAX_CHAT_PATH.'lib/class/CustomAJAXChat.php');
require(AJAX_CHAT_PATH.'lib/class/CustomAJAXChatShoutBox.php');
require(AJAX_CHAT_PATH.'lib/class/CustomAJAXChatInterface.php');

define( '_JEXEC', 1 );

define( 'DS', DIRECTORY_SEPARATOR );
define('JPATH_BASE', dirname(dirname(dirname(__FILE__))));

require_once( JPATH_BASE .DS.'includes'.DS.'defines.php' );
require_once( JPATH_BASE .DS.'includes'.DS.'framework.php' );
require_once( JPATH_BASE .DS.'libraries'.DS.'joomla'.DS.'factory.php');

$mainframe = JFactory::getApplication('site');
$mainframe->initialise();

define('J_PREFIX', $mainframe->getCfg('dbprefix'));
?>
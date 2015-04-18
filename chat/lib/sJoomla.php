<?php

define( '_JEXEC', 1);

define( 'DS', DIRECTORY_SEPARATOR );
define('JPATH_BASE', dirname(SCHAT_PATH));

require_once( JPATH_BASE .DS.'includes'.DS.'defines.php' );
require_once( JPATH_BASE .DS.'includes'.DS.'framework.php' );
require_once( JPATH_BASE .DS.'libraries'.DS.'joomla'.DS.'factory.php');
//require_once( JPATH_BASE .DS. 'components' . DS . 'com_community' . DS . 'libraries' . DS . 'core.php');

$mainframe = JFactory::getApplication('site');
$mainframe->initialise();

define('J_PREFIX', $mainframe->getCfg('dbprefix'));
Config::$dbConnection['link'] = JFactory::getDBO()->getConnection();
Config::$dbConnection['name']  = $mainframe->getCfg('db');
Config::$chatClosed  = Config::$chatClosed  || $mainframe->getCfg('offline');
Config::$gzipEnabled = Config::$gzipEnabled || $mainframe->getCfg('qzip');
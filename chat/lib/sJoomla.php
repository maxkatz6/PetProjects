<?php

define( '_JEXEC', 1);

define( 'DS', DIRECTORY_SEPARATOR );
define('JPATH_BASE', dirname(SCHAT_PATH));

require_once( JPATH_BASE .DS.'includes'.DS.'defines.php' );
require_once( JPATH_BASE .DS.'includes'.DS.'framework.php' );
require_once( JPATH_BASE .DS.'libraries'.DS.'joomla'.DS.'factory.php');

$mainframe = JFactory::getApplication('site');
$mainframe->initialise();

if ((!file_exists( JPATH_SITE . '/libraries/CBLib/CBLib/Core/CBLib.php' ) )
    || (!file_exists( JPATH_ADMINISTRATOR . '/components/com_comprofiler/plugin.foundation.php' ) ) ) {
	echo 'CB not installed'; return;
}

include_once( JPATH_ADMINISTRATOR . '/components/com_comprofiler/plugin.foundation.php' );

define('J_PREFIX', $mainframe->getCfg('dbprefix'));

Config::$dbConnection['host']  = $mainframe->getCfg('host');
Config::$dbConnection['user']  = $mainframe->getCfg('user');
Config::$dbConnection['pass']  = $mainframe->getCfg('password');
Config::$dbConnection['name']  = $mainframe->getCfg('db');
Config::$dbConnection['type']  = $mainframe->getCfg('dbtype');
Config::$dbConnection['link'] = JFactory::getDBO()->getConnection();

Config::$chatClosed  = Config::$chatClosed  || $mainframe->getCfg('offline');
Config::$gzipEnabled = Config::$gzipEnabled || $mainframe->getCfg('qzip');
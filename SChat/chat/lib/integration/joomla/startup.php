<?php
/*
 * Use this file to define globals and load custom libraries that need global scope.
 * This file is loaded before all other AJAX Chat classes by the core index.php file.
 * It is not referenced anywhere else.
*/
define( '_JEXEC', 1 );

define( 'DS', DIRECTORY_SEPARATOR );
define('JPATH_BASE', dirname(dirname(dirname(dirname(dirname(__FILE__))))));

require_once( JPATH_BASE .DS.'includes'.DS.'defines.php' );
require_once( JPATH_BASE .DS.'includes'.DS.'framework.php' );
require_once( JPATH_BASE .DS.'libraries'.DS.'joomla'.DS.'factory.php');

$mainframe = JFactory::getApplication('site');
$mainframe->initialise();

define('J_PREFIX', $mainframe->getCfg('dbprefix'));

function getTIM($t)
{
	switch($t)
	{
		case "Дон Кихот":
			return "1ile";
		case "Дюма":
			return "1sei";
		case "Робеспьер":
			return "1lii";
		case "Гюго":
			return "1ese";
		
		case "Гамлет":
			return "2eie";
		case "Максим Горький":
			return "2lsi";
		case "Жуков":
			return "2sle";
		case "Есенин":
			return "2iei";
		
		case "Бальзак":
			return "3ili";
		case "Наполеон":
			return "3see";
		case "Драйзер":
			return "3esi";
		case "Джек Лондон":
			return "3lie";
		
		case "Штирлиц":
			return "4lse";
		case "Достоевский":
			return "4eii";
		case "Гексли":
			return "4iee";
		case "Габен":
			return "4sli";
		
		default:
			return "none";
	}
}
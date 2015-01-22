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
		case "��� �����":
			return "1ile";
		case "����":
			return "1sei";
		case "���������":
			return "1lii";
		case "����":
			return "1ese";
		
		case "������":
			return "2eie";
		case "������ �������":
			return "2lsi";
		case "�����":
			return "2sle";
		case "������":
			return "2iei";
		
		case "�������":
			return "3ili";
		case "��������":
			return "3see";
		case "�������":
			return "3esi";
		case "���� ������":
			return "3lie";
		
		case "�������":
			return "4lse";
		case "�����������":
			return "4eii";
		case "������":
			return "4iee";
		case "�����":
			return "4sli";
		
		default:
			return "none";
	}
}
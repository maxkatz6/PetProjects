<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */
// Load configuration
require_once(SCHAT_PATH.DS.'lib'.DS.'config.php');
require_once(SCHAT_PATH.DS.'lib'.DS.'sJoomla.php');

// Include Class libraries:
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChat.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatDataBaseMySQLi.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatMySQLiQuery.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatDataBaseMySQL.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatMySQLQuery.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatEncoding.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatString.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatFileSystem.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatHTTPHeader.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatLanguage.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'SChatTemplate.php');
require(SCHAT_PATH.DS.'lib'.DS.'class'.DS.'JoomlaSChat.php');

if (Config::debug) {
    require_once(SCHAT_PATH.DS.'debug'.DS.'FirePHP.class.php');
    if (function_exists('xhprof_enable')){
        require_once(SCHAT_PATH.DS.'debug'.DS.'xhprof-0.9.4'.DS.'xhprof_lib'.DS.'utils'.DS.'xhprof_lib.php');
        require_once(SCHAT_PATH.DS.'debug'.DS.'xhprof-0.9.4'.DS.'xhprof_lib'.DS.'utils'.DS.'xhprof_runs.php');
        xhprof_enable(XHPROF_FLAGS_CPU + XHPROF_FLAGS_MEMORY);
    }
}
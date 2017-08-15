<?php
/*
 * @package SocioPARTY
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */
// Load configuration
require_once(SCHAT_PATH.'lib/config.php');
require_once(SCHAT_PATH.'lib/sJoomla.php');

// Include Class libraries:
require(SCHAT_PATH.'lib/class/SChat.php');
require(SCHAT_PATH.'lib/class/SChatDataBaseMySQLi.php');
require(SCHAT_PATH.'lib/class/SChatMySQLiQuery.php');
require(SCHAT_PATH.'lib/class/SChatDataBaseMySQL.php');
require(SCHAT_PATH.'lib/class/SChatMySQLQuery.php');
require(SCHAT_PATH.'lib/class/SChatEncoding.php');
require(SCHAT_PATH.'lib/class/SChatString.php');
require(SCHAT_PATH.'lib/class/SChatFileSystem.php');
require(SCHAT_PATH.'lib/class/SChatHTTPHeader.php');
require(SCHAT_PATH.'lib/class/SChatLanguage.php');
require(SCHAT_PATH.'lib/class/SChatTemplate.php');
require(SCHAT_PATH.'lib/class/JoomlaSChat.php');

if (Config::debug) {
    require_once(SCHAT_PATH.'debug/FirePHP.class.php');
    if (function_exists('xhprof_enable')){
        require_once(SCHAT_PATH.'debug/xhprof-0.9.4/xhprof_lib/utils/xhprof_lib.php');
        require_once(SCHAT_PATH.'debug/xhprof-0.9.4/xhprof_lib/utils/xhprof_runs.php');
        xhprof_enable(XHPROF_FLAGS_CPU + XHPROF_FLAGS_MEMORY);
    }
}
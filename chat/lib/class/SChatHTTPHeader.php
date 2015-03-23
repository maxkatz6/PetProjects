<?php
/*
 * @package AJAX_Chat
 * @author Sebastian Tschan
 * @copyright (c) Sebastian Tschan
 * @license Modified MIT License
 * @link https://blueimp.net/ajax/
 */

// Class to manage HTTP header
class SChatHTTPHeader {

	var $_contentType;
	var $_constant;
	var $_noCache;

	function __construct($encoding='UTF-8', $contentType='text/html', $noCache=true) {
                $this->_contentType = $contentType.'; charset='.$encoding;
                $this->_constant = true;
		$this->_noCache = $noCache;
	}

	// Method to send the HTTP header:
	function send() {
		// Prevent caching:
		if($this->_noCache) {
			header('Cache-Control: no-cache, must-revalidate');
			header('Expires: Mon, 26 Jul 1997 05:00:00 GMT');
		}
		
		// Send the content-type-header:
		header('Content-Type: '.$this->_contentType);
		
		// Send vary header if content-type varies (important for proxy-caches):
		if(!$this->_constant) {
			header('Vary: Accept');
		}
	}
    
	// Method to return the content-type string:
	function getContentType() {
		// Return the content-type string:
		return $this->_contentType;
	}

}
?>
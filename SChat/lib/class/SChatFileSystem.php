<?php
// Class to provide methods for file system access:
class SChatFileSystem {

	public static function getFileContents($file) {
		if(function_exists('file_get_contents')) {
			return file_get_contents($file);
		} else {
			return(implode('', file($file)));
		}
	}

}
?>
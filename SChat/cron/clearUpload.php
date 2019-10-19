<?php
$folderName = dirname(__DIR__).DIRECTORY_SEPARATOR."upload";
$maxAge = 2*24*60*60;

if (file_exists($folderName)) {
    foreach (new DirectoryIterator($folderName) as $fileInfo) {
        if ($fileInfo->isDot()) {
        continue;
        }
        if ($fileInfo->isFile() && time() - $fileInfo->getCTime() >= $maxAge) {
            unlink($fileInfo->getRealPath());
        }
    }
}
?>
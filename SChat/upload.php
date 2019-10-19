<?php
define('DS', DIRECTORY_SEPARATOR );

require_once  __DIR__.DS."vendor".DS."samayo".DS."bulletproof".DS."src".DS."bulletproof.php";

if ($_FILES == null
    || !isset($_FILES["file"]))
{
    echo json_encode(array("error" => "no-files"));
    die();
}

$image = (new Bulletproof\Image($_FILES))
    ->setSize(/*min*/ 0, /*max bytes*/ 10*1024*1024)
    ->setLocation(__DIR__."/upload");

if($image["file"]){
    $upload = $image->upload();

    if($upload){
        echo json_encode(array("path" => str_replace(__DIR__."/", "", $upload->getFullPath())));
    }else{
        echo json_encode(array("path" => $image["error"]));
    }
}
else {
    echo json_encode(array("error" => "no-pictures"));
}
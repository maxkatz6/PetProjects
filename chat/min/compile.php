<?php

define('DS', DIRECTORY_SEPARATOR );
$minFolder = __DIR__.DS;
$cssFolder = $minFolder.'..'.DS.'css'.DS;
$jsFolder = $minFolder.'..'.DS.'js'.DS;
require_once($minFolder.'..'.DS.'vendor'.DS.'autoload.php');

echo "Init\r\n";

use Patchwork\JSqueeze;

use Leafo\ScssPhp\Compiler;

$appJS = ["config.js","helper.js","chat.js"];
$appJSname = "app.min.js";

$mediaJS = ["jquery-ui.min.js","jquery.jplayer.min.js","player.js","webRtcAdapter.js","simplewebrtc.js","webcam.js"];
$mediaJSname = "media.min.js";

$styleCSS = ["global.css","fonts.css"];
$styleCSSname = "style.min.css";

$jz = new JSqueeze();

$scss = new Compiler();
$scss->setImportPaths($cssFolder);

echo "Start compiling\r\n";

// APP.js
$completeJS = '';
foreach ($appJS as $file){
    $completeJS .= $jz->squeeze(
        file_get_contents($jsFolder.$file),
        true,   // $singleLine
        false,   // $keepImportantComments
        false   // $specialVarRx
    );
}
file_put_contents($minFolder.$appJSname, $completeJS);
echo "Compiled ".$minFolder.$appJSname."\r\n";

// MEDIA.js
$completeJS = '';
foreach ($mediaJS as $file){
    $completeJS .= $jz->squeeze(
        file_get_contents($jsFolder.$file),
        true,   // $singleLine
        false,   // $keepImportantComments
        false   // $specialVarRx
    );
}
file_put_contents($minFolder.$mediaJSname, $completeJS);
echo "Compiled ".$minFolder.$mediaJSname."\r\n";

// STYLE.css
$completeCSS = '';
foreach ($styleCSS as $file){
    $completeCSS .= $scss->compile(file_get_contents($cssFolder.$file));
}
file_put_contents($minFolder.$styleCSSname, $completeCSS);
echo "Compiled ".$minFolder.$styleCSSname."\r\n";

echo "Success";
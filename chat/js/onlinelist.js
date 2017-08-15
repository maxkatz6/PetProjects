//<![CDATA[
var req, i;
try{
    req=new ActiveXObject("Msxml2.XMLHTTP");
} catch(e){
    try{
        req=new ActiveXObject("Microsoft.XMLHTTP");
    } catch(E){
        req=false;
    }
}
if(!req&&typeof XMLHttpRequest!='undefined') req=new XMLHttpRequest();
req.onreadystatechange=function(){
    if(req.readyState===4&&req.status===200){
        var json=JSON.parse(req.responseText).users, innHTML='<ul class="cResetList cThumbsList clearfix">';
        for(i=0; i<json.length; i++){
            var id=json[i].id, name=json[i].name, av=json[i].info.avatar, room=json[i].roomName;
            innHTML+="<li><a href=\"/index.php/jomsocial/index.php?option=com_community&view=profile&userid="+id+"/profile/\"><img class=\"cAvatar jomNameTips\" src=\""+av+"\" alt=\""+name+"\" original-title=\""+name+"\" title=\""+room+"\"></a></li>";
        }
        document.getElementById('usersOnlineSC').innerHTML=innHTML+"</ul>";
    }
};
req.open('GET', 'chat/index.php?ajax=true&view=teaser&tmc=0', true);
req.send(null);
//]]>
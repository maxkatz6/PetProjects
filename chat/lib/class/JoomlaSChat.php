<?php

// Load configuration
class JoomlaSChat extends SChat {

    function getValidLoginUserData() {
        $user = JFactory::getUser();

        if($user->get('id') != 0) {
            $userData = [
                'userID'=>$user->get('id'),
                'userName'=>str_replace(' ', /*html_entity_decode('&nbsp;')*/ '_' ,$user->get('username'))
            ];

            if (is_array($user->groups)){
                if (isset($user->groups[8]) || isset($user->groups[7])){
                    $userData['userRole'] = SCHAT_ADMIN;
                }
                else if (isset($user->groups[6])) {
                    $userData['userRole'] = SCHAT_MODERATOR;
                }
                else {$userData['userRole'] = SCHAT_USER;}
            }
            else {
                $userData['userRole'] = SCHAT_USER;
            }

            $userData['userInfo'] = ['s'=>0];

            $cbUser = CBuser::getMyInstance();

            $userData['userInfo']['tim'] = static::getTIM(mb_convert_encoding( $cbUser->getField('cb_1'), 'UTF-8'));
            $userData['userInfo']['gender'] = static::getGender(mb_convert_encoding( $cbUser->getField('cb_2'), 'UTF-8'));

            $avatar = $cbUser->getField('avatar', null, 'php');
            $hasAvatar = !is_null($avatar)
                && !is_null($avatar['avatar'])
                && $avatar['avatar'] != '';

            $userData['userInfo']['avatar'] = $hasAvatar
                ? explode("chat", $avatar['avatar'])[1]
                : static::getAvatarByGender($userData['userInfo']['gender']);

            $useragent=$_SERVER['HTTP_USER_AGENT'];
            $userData['mob'] = preg_match('/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i',$useragent)||preg_match('/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i',substr($useragent,0,4)) ? 1 : 0;

            return $userData;
        } else {
            // Returns null if login is invalid
            return null;
        }
    }

    function &getChannels() {
        if($this->_channels === null) {
            $this->_channels = array();
            if($this->getUserRole() >= SCHAT_MODERATOR) {
                $validChannels = array(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14);
            } else {
                $tim = $this->getUserInfo()['tim'];
                switch ($tim{0})
                {
                    case "1": $validChannels= array(0,1,2,3,4,5,6,7,8,9,10); break;
                    case "2": $validChannels= array(0,1,2,3,4,5,6,7,8,9,11); break;
                    case "3": $validChannels= array(0,1,2,3,4,5,6,7,8,9,12); break;
                    case "4": $validChannels= array(0,1,2,3,4,5,6,7,8,9,13); break;
                    default : $validChannels= array(0,1,2,3,4,5,6,7,8,9   ); break;
                }
            }
            foreach($this->getAllChannels() as $key=>$value) {
                if ($value == Config::defaultChannelID) {
                    $this->_channels[$key] = $value;
                    continue;
                }
                if(in_array($value, $validChannels)) {
                    $this->_channels[$key] = $value;
                }
            }
        }
        return $this->_channels;
    }

    function incrementMessageCount(){
        $id = $this->getUserID();
        if (!is_null($id)){
            $this->db->sqlQuery("UPDATE ".J_PREFIX."comprofiler SET cb_3 = cb_3 + 1 WHERE user_id = ".$id);
        }
    }

    function appendMinutesToUserTimeInChat($minutes){
        $id = $this->getUserID();
        if (!is_null($id)){
            $this->db->sqlQuery("UPDATE ".J_PREFIX."comprofiler SET cb_timeinchat = cb_timeinchat + ".$minutes." WHERE user_id = ".$id);
        }
    }

    function getUsersCount() {
        $sql = "SELECT COUNT(id) as count FROM ".J_PREFIX."users";

        $result = $this->db->sqlQuery($sql);

        if($result->error()) {
		    echo $result->getError();
		    die();
	    }

        return (int)$result->fetch()["count"];
    }

    function getUsersTopTable($skip, $count, $sortBy, $asc){
        $allowed = ["id", "username", "registerDate", "lastvisitDate", "tim", "gender", "msgCount", "minutesInChat"];

        if (!in_array($sortBy, $allowed, true))
            $sortBy = "username";

        $sql = "SELECT J.id, J.username, J.registerDate, J.lastvisitDate, ".
	           "       CB.cb_1 as tim, CB.cb_2 as gender, CB.cb_3 as msgCount, CB.cb_timeinchat as minutesInChat ".
               "FROM ".J_PREFIX."users J ".
               "INNER JOIN (SELECT * FROM ".J_PREFIX."comprofiler) AS CB ON CB.user_id = J.id ".
               "ORDER BY ".$sortBy." ".$asc." ".
               "LIMIT ".$skip.", ".$count.";";

        $result = $this->db->sqlQuery($sql);

        if($result->error()) {
		    echo $result->getError();
		    die();
	    }

	    $rows = [];
	    // Add the messages in reverse order so it is ascending again:
	    while($row = $result->fetch()) {
            $rows[] = [
                'id' => (int)$row['id'],
                'username' => SChatEncoding::encodeSpecialChars($row['username']),
                'registerDate' => strtotime($row['registerDate']),
                'lastvisitDate' => strtotime($row['lastvisitDate']),
                'gender' => static::getGender($row["gender"]),
                'tim' => static::getTIM($row["tim"]),
                'msgCount' => (int)$row["msgCount"],
                'minutesInChat' => (int)$row["minutesInChat"]
            ];
	    }
	    $result->free();

	    return $rows;
    }

    function loadUserNickColor(){
        $cbUser = CBuser::getMyInstance();

        if ($cbUser == null)
            return ["#000000"];

        $str = SChatEncoding::decodeSpecialChars($cbUser->getField('cb_nickcolor'));

        if ($str == null || $str == '')
            return ["#000000"];

        return preg_split("/[, \"'\.\[\]]+/", $str, -1, PREG_SPLIT_NO_EMPTY);
    }

    function saveUserNickColor($color){
        if (count($color) == 0)
            $color[] = "#000000";

        $str = SChatEncoding::encodeSpecialChars('["'.implode('","', $color).'"]');
        $id = $this->getUserID();
        if (!is_null($id)){
            $result = $this->db->sqlQuery("UPDATE ".J_PREFIX."comprofiler SET cb_nickcolor = ".$str." WHERE user_id = ".$id);

            if($result->error()) {
                echo $result->getError();
                die();
            }
        }
    }

    function loadUserMsgColor(){
        $cbUser = CBuser::getMyInstance();

        if ($cbUser == null)
            return ["#000000"];

        $str = SChatEncoding::decodeSpecialChars($cbUser->getField('cb_msgcolor'));

        if ($str == null || $str == '')
            return ["#000000"];

        return preg_split("/[, \"'\.\[\]]+/", $str, -1, PREG_SPLIT_NO_EMPTY);
    }

    function saveUserMsgColor($color){
        if (count($color) == 0)
            $color[] = "#000000";

        $str = SChatEncoding::encodeSpecialChars('["'.implode('","', $color).'"]');
        $id = $this->getUserID();
        if (!is_null($id)){
            $result = $this->db->sqlQuery("UPDATE ".J_PREFIX."comprofiler SET cb_msgcolor = ".$str." WHERE user_id = ".$id);

            if($result->error()) {
                echo $result->getError();
                die();
            }
        }
    }

    private static function getTIM($t)
    {
        switch($t) {
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
    private static function getGender($s)
    {
        switch ($s) {
            case 'мужской':
            case 'Мужской':
                return 'm';
            case 'женский':
            case 'Женский':
                return 'f';
            default:
                return 'n';
        }
    }
    private static function getAvatarByGender($gender){
        switch ($gender){
            case 'm':
                return Config::maleAvatar;
            case 'f':
                return Config::femaleAvatar;
            default:
                return Config::defaultAvatar;
        }
    }
}
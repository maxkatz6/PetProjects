<?php
// Load configuration
class CustomSChat extends SChat {
    //переписать под CFactory::getUser( $data->id );
    function getValidLoginUserData() {
        $user = JFactory::getUser();
        if($user->get('id') != 0) {
            $userData = array();
            $userData['userID'] = $user->get('id');
            $userData['userName'] = str_replace(' ', /*html_entity_decode('&nbsp;')*/ '_' ,$user->get('name'));

            if (is_array($user->groups)){
                if (isset($user->groups[8]) || isset($user->groups[7])){
                    $userData['userRole'] = SCHAT_ADMIN;}
                else if (isset($user->groups[6])) {
                    $userData['userRole'] = SCHAT_MODERATOR;}
                else {$userData['userRole'] = SCHAT_USER;}}
            else {
                $userData['userRole'] = SCHAT_USER;
            }
            
            $userData['userInfo'] = array();

            $res = $this->db->sqlQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 17')->fetch();
            $userData['userInfo']['tim'] = self::getTIM(mb_convert_encoding( $res['value'], 'UTF-8'));
                    
            $res = $this->db->sqlQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 2')->fetch();
            $userData['userInfo']['gender'] = self::getGender(mb_convert_encoding( $res['value'], 'UTF-8'));
                                
            $res = $this->db->sqlQuery('SELECT thumb FROM '.J_PREFIX.'community_users WHERE userid = '.$userData['userID'])->fetch();
            $a = $res['thumb'];
            $userData['userInfo']['avatar'] = ( is_null($a) ? ($userData['userInfo']['gender'] == 'f' ? 'images/user-Женский-thumb.png': 'images/user-Мужской-thumb.png') : $a);

            return $userData;
        } else {
            // Returns null if login is invalid
            return null;
        }
    }

    // Store the channels the current user has access to
    // Make sure channel names don't contain any whitespace
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

    private static function getTIM($t)
    {
        switch($t)
        {
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
        switch ($s)
        {
        case'male':
        case'Мужской':
            return 'm';
        case 'female':
        case 'Женский':
            return 'f';
        default: 
            return 'n';
        }
    }
} 
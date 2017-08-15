<?php
// Load configuration
class JoomlaSChat extends SChat {
    //переписать под CFactory::getUser( $data->id );
    function getValidLoginUserData() {
        $user = JFactory::getUser();
        if($user->get('id') != 0) {
            $userData = [
                'userID'=>$user->get('id'),
                'userName'=>str_replace(' ', /*html_entity_decode('&nbsp;')*/ '_' ,$user->get('name'))
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
            
            $res = $this->db->sqlQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 17')->fetch();
            $userData['userInfo']['tim'] = self::getTIM(mb_convert_encoding( $res['value'], 'UTF-8'));
            
            $res = $this->db->sqlQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 2')->fetch();
            $userData['userInfo']['gender'] = self::getGender(mb_convert_encoding( $res['value'], 'UTF-8'));
            
            $res = $this->db->sqlQuery('SELECT thumb FROM '.J_PREFIX.'community_users WHERE userid = '.$userData['userID'])->fetch();
            $a = $res['thumb'];
            $userData['userInfo']['avatar'] = ( is_null($a) || $a == ''? ($userData['userInfo']['gender'] == 'f' ? 'images/user-Женский-thumb.png': 'images/user-Мужской-thumb.png') : $a);

            $useragent=$_SERVER['HTTP_USER_AGENT'];
            $userData['mob'] = preg_match('/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i',$useragent)||preg_match('/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i',substr($useragent,0,4)) ? 1 : 0;
            
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
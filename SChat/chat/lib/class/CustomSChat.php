<?php
// Load configuration
class CustomSChat extends SChat {

    var $jdb;
    function __construct() {
        $this->jdb = JFactory::getDBO();
        
        Config::$dbConnection['link'] = $this->jdb->getConnection();
        parent::__construct();
    }
        
    //переписать под CFactory::getUser( $data->id );
    function getValidLoginUserData() {
        $user = JFactory::getUser();
        if($user->get('id') != 0) {
            $userData = array();
            $userData['userID'] = $user->get('id');
            $userData['userName'] = str_replace(' ', /*html_entity_decode('&nbsp;')*/ '_' ,$user->get('name'));
            
            if (is_array($user->groups) && (isset($user->groups[8]) || isset($user->groups[7]))){
                $userData['userRole'] = SCHAT_ADMIN;}
            else if (isset($user->groups[7])) {
                $userData['userRole'] = SCHAT_MODERATOR;}
            else {
                $userData['userRole'] = SCHAT_USER;
            }
            
            $userData['userInfo'] = array();

            
            
            $this->jdb->setQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 17');
            $userData['userInfo']['tim'] = $this->getTIM(mb_convert_encoding( $this->jdb->loadResult(), 'UTF-8'));
                            //SELECT value FROM jml_community_fields_values WHERE user_id = 174 AND field_id = 2
            $this->jdb->setQuery('SELECT value FROM '.J_PREFIX.'community_fields_values WHERE user_id = '.$userData['userID'].' AND field_id = 2');
            $userData['userInfo']['gender'] = $this->getGender(mb_convert_encoding( $this->jdb->loadResult(), 'UTF-8'));
                                
            $this->jdb->setQuery('SELECT thumb FROM '.J_PREFIX.'community_users WHERE userid = '.$userData['userID']);
            $a = $this->jdb->loadResult();
            $userData['userInfo']['avatar'] = ( is_null($a) ? ($userData['userInfo']['gender'] == 'f' ? '/images/user-Женский-thumb.png': '/images/user-Мужской-thumb.png') : $a);

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
            if($this->getUserRole() == SCHAT_ADMIN || $this->getUserRole() == SCHAT_MODERATOR) {
                $validChannels = array(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14);
            } else {	
                $tim = json_decode($this->getUserInfo())->tim;
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
                if(Config::limitChannelList && !in_array($value, Config::limitChannelList)) {
                    continue;
                }
                if(in_array($value, $validChannels)) {
                    $this->_channels[$key] = $value;
                }
            }
        }
        return $this->_channels;
    }

    // Store all existing channels
    // Make sure channel names don't contain any whitespace
    function &getAllChannels() {
        if($this->_allChannels === null) {
            // Get all existing channels:
            $customChannels = array_flip(Config::$channels);

            $defaultChannelFound = false;

            foreach($customChannels as $name=>$id) {
                $this->_allChannels[$this->trimChannelName($name)] = $id;
                if($id == Config::defaultChannelID) {
                    $defaultChannelFound = true;
                }
            }

            if(!$defaultChannelFound) {
                // Add the default channel as first array element to the channel list
                // First remove it in case it appeard under a different ID
                unset($this->_allChannels[Config::defaultChannelName]);
                $this->_allChannels = array_merge(
                        array($this->trimChannelName(Config::defaultChannelName)=>Config::defaultChannelID),
                              $this->_allChannels);
            }
        }
        return $this->_allChannels;
    }

    function getTIM($t)
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
    function getGender($s)
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
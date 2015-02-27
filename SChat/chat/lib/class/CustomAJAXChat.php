<?php
// Load configuration
class CustomAJAXChat extends AJAXChat {

    //переписать под CFactory::getUser( $data->id );
	function getValidLoginUserData() {
		$user = JFactory::getUser();
		if($user->get('id') != 0) {
			$userData = array();
			$userData['userID'] = $user->get('id');
			$userData['userName'] = str_replace(' ','_',$user->get('name'));
			$userData['userRole'] = ((is_array($user->groups))? ((isset($user->groups[8]) || isset($user->groups[7])) ? AJAX_CHAT_ADMIN : AJAX_CHAT_USER) : AJAX_CHAT_GUEST);
			$userData['userInfo'] = array();

			$db = JFactory::getDBO();
			
			$db->setQuery('SELECT value FROM '.J_PREFIX."community_fields_values".' WHERE user_id = '.$userData['userID'].' AND field_id = 17');
                        $userData['userInfo']['tim'] = $this->getTIM(mb_convert_encoding( $db->loadResult(), 'UTF-8'));
                        
                        $db->setQuery('SELECT thumb FROM '.J_PREFIX."community_users".' WHERE userid = '.$userData['userID']);
			$a = $db->loadResult();
			$userData['userInfo']['avatar'] = ( is_null($a) ? "anon" : $a);

			return $userData;
		} else {
			// Returns null if login is invalid
			return null;
		}
	}
/*			$tim = $db->loadResult();
                        $tim1 = $this->getTIM($tim);
                        $tim2 = $this->getTIM(mb_convert_encoding($tim, 'UTF-8', 'Windows-1251'));
                        $userData['userInfo']['tim'] = ($tim1 == 'none' ? $tim2 : $tim1);*/
	// Store the channels the current user has access to
	// Make sure channel names don't contain any whitespace
	function &getChannels() {
		if($this->_channels === null) {
			$this->_channels = array();
			if($this->getUserRole() == AJAX_CHAT_ADMIN) {
				$validChannels = array(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14);
			} else {
				
				$tim = json_decode($this->getUserInfo())->tim;
				switch ($tim{0} )
				{
					case "1": $validChannels= array(0,1,2,3,4,5,6,7,8,9,10); break;
					case "2": $validChannels= array(0,1,2,3,4,5,6,7,8,9,11); break;
					case "3": $validChannels= array(0,1,2,3,4,5,6,7,8,9,12); break;
					case "4": $validChannels= array(0,1,2,3,4,5,6,7,8,9,13); break;
					default: $validChannels = array(0,1,2,3,4,5,6,7,8,9   ); break;
				}
			}
			foreach($this->getAllChannels() as $key=>$value) {
				if ($value == $this->getConfig('defaultChannelID')) {
					$this->_channels[$key] = $value;
					continue;
				}
				if($this->getConfig('limitChannelList') && !in_array($value, $this->getConfig('limitChannelList'))) {
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
			$customChannels = array_flip($this->getConfig('channels'));

			$defaultChannelFound = false;

			foreach($customChannels as $name=>$id) {
				$this->_allChannels[$this->trimChannelName($name)] = $id;
				if($id == $this->getConfig('defaultChannelID')) {
					$defaultChannelFound = true;
				}
			}

			if(!$defaultChannelFound) {
				// Add the default channel as first array element to the channel list
				// First remove it in case it appeard under a different ID
				unset($this->_allChannels[$this->getConfig('defaultChannelName')]);
				$this->_allChannels = array_merge(
					array(
							$this->trimChannelName($this->getConfig('defaultChannelName'))=>$this->getConfig('defaultChannelID')
							),
						$this->_allChannels
						);
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
    }

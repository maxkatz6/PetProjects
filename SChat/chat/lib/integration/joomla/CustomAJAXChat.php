<?php

class CustomAJAXChat extends AJAXChat {

	function getValidLoginUserData() {
		$user = JFactory::getUser();
		if($user->get('id') != 0) {
			$userData = array();
			$userData['userID'] = $user->get('id');
			$userData['userName'] = str_replace(' ','_',$user->get('name'));
			$userData['userRole'] = ((is_array($user->groups))? ((isset($user->groups[8]) || isset($user->groups[7])) ? AJAX_CHAT_ADMIN : AJAX_CHAT_USER) : AJAX_CHAT_GUEST);

			$db = JFactory::getDBO();
			
			$db->setQuery('SELECT alias FROM '.J_PREFIX."community_users".' WHERE userid = '.$userData['userID']);
			$userData['userProfile'] = $db->loadResult();     
			
			$db->setQuery('SELECT value FROM '.J_PREFIX."community_fields_values".' WHERE user_id = '.$userData['userID'].' AND field_id = 17');
			$tim = iconv("UTF-8", "CP1251", $db->loadResult());
			$userData['userTIM'] = $this->getTIM($tim);
			
			$db->setQuery('SELECT thumb FROM '.J_PREFIX."community_users".' WHERE userid = '.$userData['userID']);
			$a = $db->loadResult();
			$userData['userAvatar'] = ( is_null($a) ? "anon" : $a);

			// Returns an associative array containing userName, userID and userRole
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
			
			// Get the channels, the user has access to:
			if($this->getUserRole() == AJAX_CHAT_ADMIN) {
				$validChannels = array(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14);
			} else {
				
				$tim = $this->getUserTIM();
				switch ($tim{0} )
				{
					case "1": $validChannels= array(0,1,2,3,4,5,6,7,8,9,10); break;
					case "2": $validChannels= array(0,1,2,3,4,5,6,7,8,9,11); break;
					case "3": $validChannels= array(0,1,2,3,4,5,6,7,8,9,12); break;
					case "4": $validChannels= array(0,1,2,3,4,5,6,7,8,9,13); break;
					default: $validChannels = array(0,1,2,3,4,5,6,7,8,9   ); break;
				}
			}

			// Add the valid channels to the channel list (the defaultChannelID is always valid):
			foreach($this->getAllChannels() as $key=>$value) {
				if ($value == $this->getConfig('defaultChannelID')) {
					$this->_channels[$key] = $value;
					continue;
				}
				// Check if we have to limit the available channels:
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
			$customChannels = $this->getCustomChannels();

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

	function getCustomChannels() {
		// List containing the custom channels:
		$channels = null;
		require(AJAX_CHAT_PATH.'lib/data/channels.php');
		// Channel array structure should be:
		// ChannelName => ChannelID
		return array_flip($channels);
	}

	function getTIM($t)
	{
		switch($t)
		{
			case "��� �����":
				return "1ile";
			case "����":
				return "1sei";
			case "���������":
				return "1lii";
			case "����":
				return "1ese";
			
			case "������":
				return "2eie";
			case "������ �������":
				return "2lsi";
			case "�����":
				return "2sle";
			case "������":
				return "2iei";
			
			case "�������":
				return "3ili";
			case "��������":
				return "3see";
			case "�������":
				return "3esi";
			case "���� ������":
				return "3lie";
			
			case "�������":
				return "4lse";
			case "�����������":
				return "4eii";
			case "������":
				return "4iee";
			case "�����":
				return "4sli";
			
			default:
				return "none";
		}
	}
}
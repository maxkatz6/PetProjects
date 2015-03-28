<?php
// Class to provide static encoding methods
class SChatEncoding {

	// Helper function to store special chars as we cannot use static class members in PHP4:
	public static function getSpecialChars() {
		static $specialChars;
		if(!$specialChars) {
			// As &apos; is not supported by IE, we use &#39; as replacement for "'":
			$specialChars = array('&'=>'&amp;', '<'=>'&lt;', '>'=>'&gt;', "'"=>'&#39;', '"'=>'&quot;');	
		}
		return $specialChars;
	}

	// Helper function to store Regular expression for NO-WS-CTL as we cannot use static class members in PHP4:
	public static function getRegExp_NO_WS_CTL() {
		static $regExp_NO_WS_CTL;
		if(!$regExp_NO_WS_CTL) {
			// Regular expression for NO-WS-CTL, non-whitespace control characters (RFC 2822), decimal 1–8, 11–12, 14–31, and 127:
			$regExp_NO_WS_CTL = '/[\x0\x1\x2\x3\x4\x5\x6\x7\x8\xB\xC\xE\xF\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F]/';
		}
		return $regExp_NO_WS_CTL;
	}

	public static function htmlEncode($str) {
		return self::encodeSpecialChars($str);
	}

	public static function encodeSpecialChars($str) {
		return strtr($str, self::getSpecialChars());
	}

	public static function decodeSpecialChars($str) {
		return strtr($str, array_flip(self::getSpecialChars()));
	}

	public static function encodeEntities($str, $encoding='UTF-8', $convmap=null) {
		if($convmap && function_exists('mb_encode_numericentity')) {
			return mb_encode_numericentity($str, $convmap, $encoding);
		}
		return htmlentities($str, ENT_QUOTES, $encoding);
	}

	public static function decodeEntities($str, $encoding='UTF-8', $htmlEntitiesMap=null) {
		// Due to PHP bug #25670, html_entity_decode does not work with UTF-8 for PHP versions < 5:
		if(function_exists('html_entity_decode') && version_compare(phpversion(), 5, '>=')) {
			// Replace numeric and literal entities:
			$str = html_entity_decode($str, ENT_QUOTES, $encoding);
			// Replace additional literal HTML entities if an HTML entities map is given:
			if($htmlEntitiesMap) {
				$str = strtr($str, $htmlEntitiesMap);
			}
		} else {
			// Replace numeric entities:
			$str = preg_replace('~&#([0-9]+);~e', 'SChatEncoding::unicodeChar("\\1")', $str);
			$str = preg_replace('~&#x([0-9a-f]+);~ei', 'SChatEncoding::unicodeChar(hexdec("\\1"))', $str);
			// Replace literal entities:
			$htmlEntitiesMap = $htmlEntitiesMap ? $htmlEntitiesMap : array_flip(get_html_translation_table(HTML_ENTITIES, ENT_QUOTES));
			$str = strtr($str, $htmlEntitiesMap);
		}
		return $str;
	}

	public static function unicodeChar($c) {
		if($c <= 0x7F) {
			return chr($c);
		} else if($c <= 0x7FF) {
			return chr(0xC0 | $c >> 6) . chr(0x80 | $c & 0x3F);
		} else if($c <= 0xFFFF) {
			return chr(0xE0 | $c >> 12) . chr(0x80 | $c >> 6 & 0x3F)
										. chr(0x80 | $c & 0x3F);
		} else if($c <= 0x10FFFF) {
			return chr(0xF0 | $c >> 18) . chr(0x80 | $c >> 12 & 0x3F)
										. chr(0x80 | $c >> 6 & 0x3F)
										. chr(0x80 | $c & 0x3F);
		} else {
			return null;
		}
	}

	public static function removeUnsafeCharacters($str) {
		// Remove NO-WS-CTL, non-whitespace control characters (RFC 2822), decimal 1–8, 11–12, 14–31, and 127:
		return preg_replace(self::getRegExp_NO_WS_CTL(), '', $str);
	}

}
?>
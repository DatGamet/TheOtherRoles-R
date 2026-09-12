import os
import json

WORKING_DIR = os.path.dirname(os.path.realpath(__file__))
IN_DIR = os.path.join(WORKING_DIR, "Translations")
OUT_FILE = os.path.join(WORKING_DIR, "TheOtherRoles", "Resources", "stringData.json")

# Maps a Translations/<code>.json filename to the numeric language id expected
# by AmongUs.Data.SupportedLangs / ModTranslation.cs.
LANG_IDS = {
    "en": 0,
    "es-419": 1,
    "pt-br": 2,
    "pt": 3,
    "ko": 4,
    "ru": 5,
    "nl": 6,
    "fil": 7,
    "fr": 8,
    "de": 9,
    "it": 10,
    "ja": 11,
    "es": 12,
    "zh-cn": 13,
    "zh-tw": 14,
    "ga": 15,
}


def translationsToJson():
    stringData = {}

    for code, langId in LANG_IDS.items():
        path = os.path.join(IN_DIR, f"{code}.json")
        if not os.path.isfile(path):
            continue

        with open(path, "r", encoding="utf-8") as f:
            entries = json.load(f)

        for name, text in entries.items():
            if not text:
                continue
            stringData.setdefault(name, {})[str(langId)] = text

    with open(OUT_FILE, "w", newline="\n") as f:
        json.dump(stringData, f, indent=4)


if __name__ == "__main__":
    translationsToJson()

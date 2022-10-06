# PasswordStats

This tool performs some statistics on passwords cracked with HashCat.
Your input file needs to be created with: "hashcat -m <your cracking option> <your hash file> --show --username"

The tool analyses the length, as well as how the password is constructed:
             * only letters
             * lower & uppercase
             * only numbers
             * letters & numbers
             * also special chars used

@echo off
echo Generate resx files from google sheet.
csvtrans -s 1ikjbnQJWzCrwdf2Zr_XXxbxP-H-KVTGIBOXTjp-Vqno Resx -f resx -o ./Properties -n strings
echo Copy english to default file.
mv ./Properties/strings.en.resx ./Properties/strings.resx --verbose
pause >nul
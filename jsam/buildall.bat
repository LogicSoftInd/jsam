rm -rf bin\release

pushd .
dotnet publish -c release -r win10-x64
cd bin\release\netcoreapp1.1\win10-x64\publish
7z a -tzip jsam-win10-x64.zip

popd

pushd .
dotnet publish -c release -r ubuntu.16.04-x64
cd bin\release\netcoreapp1.1\ubuntu.16.04-x64\publish
7z a -tzip jsam-ubuntu.16.04-x64.zip

popd

pushd .
dotnet publish -c release -r ubuntu.16.10-x64
cd bin\release\netcoreapp1.1\ubuntu.16.10-x64\publish
7z a -tzip jsam-ubuntu.16.10-x64.zip

popd

pushd .
dotnet publish -c release -r osx.10.11-x64
cd bin\release\netcoreapp1.1\osx.10.11-x64\publish
7z a -tzip jsam-osx.10.11-x64.zip

popd

pushd .
dotnet publish -c release -r osx.10.12-x64
cd bin\release\netcoreapp1.1\osx.10.12-x64\publish
7z a -tzip jsam-osx.10.12-x64.zip

popd
cd bin\release\netcoreapp1.1
mkdir all-releases

copy win10-x64\publish\jsam-win10-x64.zip all-releases\
copy ubuntu.16.04-x64\publish\jsam-ubuntu.16.04-x64.zip all-releases\
copy ubuntu.16.10-x64\publish\jsam-ubuntu.16.10-x64.zip all-releases\
copy osx.10.11-x64\publish\jsam-osx.10.11-x64.zip all-releases\
copy osx.10.12-x64\publish\jsam-osx.10.12-x64.zip all-releases\

cd all-releases

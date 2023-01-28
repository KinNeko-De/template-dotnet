:: starts the system under test
docker network create project.1-dev-net

pushd ..\project.1.template.1
dotnet publish -o ../sut/publish -c Release --no-self-contained
popd

pushd ..\sut
docker compose --verbose up --build --remove-orphans
popd

pause

docker network create project.1-dev-net

pushd ..\database
docker compose --verbose up --remove-orphans
popd

pause
rm -rf local-test || true
mkdir -p local-test
cd local-test

dotnet new microservice --name-dotnet SvcExample --name-project Template --name-domain Example
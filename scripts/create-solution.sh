#!/usr/bin/env bash
# Recreate ShikkhaDesk.sln and wire project references with the .NET CLI.
# Requires the .NET 8 SDK. Run from the repository root:  bash scripts/create-solution.sh
set -euo pipefail
cd "$(dirname "$0")/../src"

dotnet new sln -n ShikkhaDesk

libs=(Domain Accounting Licensing Application Persistence LanTransport Reporting Backup Diagnostics)
for p in "${libs[@]}"; do dotnet sln add "ShikkhaDesk.$p/ShikkhaDesk.$p.csproj"; done
dotnet sln add ShikkhaDesk.Presentation/ShikkhaDesk.Presentation.csproj
dotnet sln add ShikkhaDesk.LicenseManager/ShikkhaDesk.LicenseManager.csproj
for t in Domain Accounting Licensing; do dotnet sln add "../tests/ShikkhaDesk.$t.Tests/ShikkhaDesk.$t.Tests.csproj"; done

echo "Solution recreated. Build/test:"
echo "  dotnet build src/ShikkhaDesk.sln -c Release"
echo "  dotnet test  src/ShikkhaDesk.sln"

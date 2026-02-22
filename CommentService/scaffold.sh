#!/bin/bash

dotnet ef dbcontext scaffold "Server=localhost,1442;Database=comments-db-real;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Entities \
  --context-dir Database \
  --context AppDbContext \
  --no-onconfiguring \
  --force
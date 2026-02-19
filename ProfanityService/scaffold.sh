#!/bin/bash

dotnet ef dbcontext scaffold "Server=localhost,1441;Database=profanity-db-real;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Entities \
  --context-dir Database \
  --context AppDbContext \
  --no-onconfiguring \
  --force
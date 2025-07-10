# Test API Gateway POC
Write-Host "🚀 Testing API Gateway POC" -ForegroundColor Green
Write-Host ""

# Test Product Service through Gateway
Write-Host "📦 Testing /product endpoint:" -ForegroundColor Yellow
try {
    $productResponse = Invoke-WebRequest -Uri "http://localhost:5000/product" -UseBasicParsing
    Write-Host "Status: $($productResponse.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($productResponse.Content)" -ForegroundColor White
    Write-Host "X-Service-Origin: $($productResponse.Headers['X-Service-Origin'])" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test User Service through Gateway
Write-Host "👤 Testing /user endpoint:" -ForegroundColor Yellow
try {
    $userResponse = Invoke-WebRequest -Uri "http://localhost:5000/user" -UseBasicParsing
    Write-Host "Status: $($userResponse.StatusCode)" -ForegroundColor Green
    Write-Host "Response: $($userResponse.Content)" -ForegroundColor White
    Write-Host "X-Service-Origin: $($userResponse.Headers['X-Service-Origin'])" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "✅ Test completed!" -ForegroundColor Green 
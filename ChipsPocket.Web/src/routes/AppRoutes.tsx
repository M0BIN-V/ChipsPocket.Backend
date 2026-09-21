import { Navigate, Route, Routes } from 'react-router-dom'
import { authStorage } from '../api/authStorage'
import { AuthenticatedPage } from '../features/auth/AuthenticatedPage'
import { LoginPage } from '../features/auth/LoginPage'

function AuthenticatedRoute() { return authStorage.getAccessToken() ? <AuthenticatedPage /> : <Navigate to="/login" replace /> }

export function AppRoutes() {
  return <Routes><Route path="/login" element={<LoginPage />} /><Route path="/authenticated" element={<AuthenticatedRoute />} /><Route path="/" element={<Navigate to="/login" replace />} /><Route path="*" element={<Navigate to="/login" replace />} /></Routes>
}

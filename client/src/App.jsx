import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import { ToastProvider } from './components/Toast'
import PrivateRoute from './components/PrivateRoute'
import Navbar from './components/Navbar'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import ProfilePage from './pages/ProfilePage'
import MatchesPage from './pages/MatchesPage'
import MatchDetailPage from './pages/MatchDetailPage'
import CreateMatchPage from './pages/CreateMatchPage'

function Layout({ children }) {
  return (
    <div className="min-h-screen bg-gray-950">
      <Navbar />
      <main className="max-w-4xl mx-auto px-4 py-6">{children}</main>
    </div>
  )
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ToastProvider>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/matches" element={
            <PrivateRoute><Layout><MatchesPage /></Layout></PrivateRoute>
          } />
          <Route path="/matches/new" element={
            <PrivateRoute><Layout><CreateMatchPage /></Layout></PrivateRoute>
          } />
          <Route path="/matches/:id" element={
            <PrivateRoute><Layout><MatchDetailPage /></Layout></PrivateRoute>
          } />
          <Route path="/profile" element={
            <PrivateRoute><Layout><ProfilePage /></Layout></PrivateRoute>
          } />
          <Route path="*" element={<Navigate to="/matches" replace />} />
        </Routes>
        </ToastProvider>
      </AuthProvider>
    </BrowserRouter>
  )
}

import { Link, useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Navbar() {
  const { user, logout, isAdmin } = useAuth()
  const navigate = useNavigate()
  const { pathname } = useLocation()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const linkClass = (path) =>
    `text-sm transition-colors ${
      pathname === path || (path !== '/matches' && pathname.startsWith(path))
        ? 'text-green-400 font-semibold'
        : 'text-gray-400 hover:text-green-400'
    }`

  return (
    <nav className="bg-gray-900 border-b border-gray-800 px-4 py-3 flex items-center justify-between sticky top-0 z-40 backdrop-blur-sm">
      <Link to="/matches" className="text-green-400 font-bold text-xl tracking-tight flex items-center gap-1.5">
        <span>⚽</span>
        <span>FulboUY</span>
      </Link>

      {user && (
        <div className="flex items-center gap-5">
          <Link to="/matches" className={linkClass('/matches')}>
            Partidos
          </Link>
          <Link to="/profile" className={linkClass('/profile')}>
            Mi Perfil
          </Link>
          {isAdmin && (
            <Link to="/matches/new" className={linkClass('/matches/new')}>
              + Partido
            </Link>
          )}
          <div className="flex items-center gap-3 pl-2 border-l border-gray-800">
            <span className="text-gray-600 text-xs hidden sm:block">{user.email}</span>
            {isAdmin && (
              <span className="text-xs bg-green-900/40 text-green-400 border border-green-700/30 px-1.5 py-0.5 rounded-full">
                Admin
              </span>
            )}
            <button
              onClick={handleLogout}
              className="text-xs text-gray-500 hover:text-red-400 transition-colors"
            >
              Salir
            </button>
          </div>
        </div>
      )}
    </nav>
  )
}

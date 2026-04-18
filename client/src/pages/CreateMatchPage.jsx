import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { createMatch } from '../api/matches'
import { useToast } from '../components/Toast'

export default function CreateMatchPage() {
  const navigate = useNavigate()
  const toast = useToast()
  const [form, setForm] = useState({
    location: '',
    date: '',
    fieldCost: '',
    maxPlayers: 10,
  })
  const [loading, setLoading] = useState(false)

  const set = (field) => (e) => setForm((f) => ({ ...f, [field]: e.target.value }))

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    try {
      const match = await createMatch({
        location: form.location,
        date: new Date(form.date).toISOString(),
        fieldCost: parseFloat(form.fieldCost),
        maxPlayers: parseInt(form.maxPlayers),
      })
      toast.success('Partido creado correctamente.')
      navigate(`/matches/${match.id}`)
    } catch (err) {
      toast.error(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="max-w-md mx-auto">
      <button
        type="button"
        onClick={() => navigate(-1)}
        className="flex items-center gap-1 text-gray-500 hover:text-green-400 text-sm transition-colors mb-4"
      >
        ← Volver
      </button>
      <h1 className="text-2xl font-bold text-white mb-6">Nuevo partido</h1>
      <form onSubmit={handleSubmit} className="bg-gray-900 rounded-xl p-6 border border-gray-800 space-y-4">

        <div>
          <label className="text-gray-400 text-sm block mb-1">Lugar</label>
          <input
            type="text"
            value={form.location}
            onChange={set('location')}
            required
            placeholder="Complejo Urbano - Cancha 3"
            className="w-full bg-gray-800 text-white rounded-lg px-3 py-2 border border-gray-700 focus:border-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="text-gray-400 text-sm block mb-1">Fecha y hora</label>
          <input
            type="datetime-local"
            value={form.date}
            onChange={set('date')}
            required
            className="w-full bg-gray-800 text-white rounded-lg px-3 py-2 border border-gray-700 focus:border-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="text-gray-400 text-sm block mb-1">Precio de la cancha ($)</label>
          <input
            type="number"
            value={form.fieldCost}
            onChange={set('fieldCost')}
            required
            min="1"
            step="0.01"
            placeholder="2000"
            className="w-full bg-gray-800 text-white rounded-lg px-3 py-2 border border-gray-700 focus:border-green-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="text-gray-400 text-sm block mb-1">
            Máximo de jugadores: <span className="text-green-400">{form.maxPlayers}</span>
          </label>
          <input
            type="range"
            value={form.maxPlayers}
            onChange={set('maxPlayers')}
            min="10"
            max="22"
            step="2"
            className="w-full accent-green-500"
          />
          <div className="flex justify-between text-gray-500 text-xs mt-1">
            <span>10 (fútbol 5)</span>
            <span>14 (fútbol 7)</span>
            <span>22 (fútbol 11)</span>
          </div>
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-green-600 hover:bg-green-500 disabled:opacity-50 text-white font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2"
        >
          {loading && (
            <span className="inline-block h-4 w-4 rounded-full border-2 border-white/30 border-t-white animate-spin" />
          )}
          {loading ? 'Creando...' : 'Crear partido'}
        </button>
      </form>
    </div>
  )
}

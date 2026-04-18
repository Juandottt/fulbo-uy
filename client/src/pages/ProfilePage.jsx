import { useEffect, useState } from 'react'
import { getMyProfile, createProfile, updateProfile } from '../api/players'
import { useToast } from '../components/Toast'
import { PageSpinner } from '../components/Spinner'

const ATTRS = [
  { key: 'speed', label: 'Velocidad', icon: '⚡' },
  { key: 'defense', label: 'Defensa', icon: '🛡️' },
  { key: 'passing', label: 'Pase', icon: '🎯' },
  { key: 'shooting', label: 'Definición', icon: '⚽' },
]

function SkillSlider({ label, icon, value, onChange }) {
  const barColor =
    value >= 8 ? 'bg-green-400' : value >= 5 ? 'bg-green-600' : 'bg-gray-600'

  return (
    <div>
      <div className="flex justify-between items-center mb-1.5">
        <span className="text-gray-400 text-sm">
          {icon} {label}
        </span>
        <span className="text-green-400 font-bold text-sm w-5 text-right">{value}</span>
      </div>
      {/* visual bar */}
      <div className="h-1.5 bg-gray-800 rounded-full mb-2 overflow-hidden">
        <div
          className={`h-full rounded-full transition-all duration-200 ${barColor}`}
          style={{ width: `${((value - 1) / 9) * 100}%` }}
        />
      </div>
      <input
        type="range"
        min="1"
        max="10"
        value={value}
        onChange={(e) => onChange(parseInt(e.target.value))}
        className="w-full accent-green-500 h-1"
      />
    </div>
  )
}

function AverageBadge({ value }) {
  const color =
    value >= 7.5 ? 'text-green-400 border-green-600/40 bg-green-900/20'
    : value >= 5 ? 'text-yellow-400 border-yellow-600/40 bg-yellow-900/20'
    : 'text-gray-400 border-gray-600/40 bg-gray-800/40'

  return (
    <div className={`border rounded-xl p-4 flex items-center justify-between ${color}`}>
      <div>
        <p className="text-xs opacity-70 mb-0.5">Promedio de habilidades</p>
        <p className="text-4xl font-bold">{value}</p>
      </div>
      <div className="text-5xl opacity-20">⚽</div>
    </div>
  )
}

export default function ProfilePage() {
  const toast = useToast()
  const [profile, setProfile] = useState(null)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)

  const [form, setForm] = useState({
    name: '',
    speed: 5,
    defense: 5,
    passing: 5,
    shooting: 5,
  })

  useEffect(() => {
    getMyProfile()
      .then((p) => {
        setProfile(p)
        setForm({ name: p.name, speed: p.speed, defense: p.defense, passing: p.passing, shooting: p.shooting })
      })
      .catch(() => setProfile(null))
      .finally(() => setLoading(false))
  }, [])

  const average = ((form.speed + form.defense + form.passing + form.shooting) / 4).toFixed(2)
  const setAttr = (key) => (val) => setForm((f) => ({ ...f, [key]: val }))

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (!form.name.trim()) { toast.error('El nombre no puede estar vacío.'); return }
    setSaving(true)
    try {
      if (profile) {
        const updated = await updateProfile(profile.id, form)
        setProfile(updated)
        toast.success('Perfil actualizado correctamente.')
      } else {
        const created = await createProfile(form)
        setProfile(created)
        toast.success('Perfil creado correctamente.')
      }
    } catch (err) {
      toast.error(err.message)
    } finally {
      setSaving(false)
    }
  }

  if (loading) return <PageSpinner />

  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-2xl font-bold text-white mb-6">Mi perfil futbolístico</h1>

      <div className="mb-6">
        <AverageBadge value={average} />
      </div>

      <form onSubmit={handleSubmit} className="bg-gray-900 rounded-xl p-6 border border-gray-800 space-y-5">
        <div>
          <label className="text-gray-400 text-sm block mb-1">Nombre en la cancha</label>
          <input
            type="text"
            value={form.name}
            onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
            required
            placeholder="Juan el Rápido"
            className="w-full bg-gray-800 text-white rounded-lg px-3 py-2 border border-gray-700 focus:border-green-500 focus:outline-none transition-colors"
          />
        </div>

        <div className="space-y-5">
          <p className="text-gray-500 text-xs uppercase tracking-wider">Atributos (1–10)</p>
          {ATTRS.map(({ key, label, icon }) => (
            <SkillSlider
              key={key}
              label={label}
              icon={icon}
              value={form[key]}
              onChange={setAttr(key)}
            />
          ))}
        </div>

        <button
          type="submit"
          disabled={saving}
          className="w-full bg-green-600 hover:bg-green-500 disabled:opacity-50 text-white font-semibold py-2.5 rounded-lg transition-colors flex items-center justify-center gap-2"
        >
          {saving && (
            <span className="inline-block h-4 w-4 rounded-full border-2 border-white/30 border-t-white animate-spin" />
          )}
          {saving ? 'Guardando...' : profile ? 'Actualizar perfil' : 'Crear perfil'}
        </button>
      </form>
    </div>
  )
}

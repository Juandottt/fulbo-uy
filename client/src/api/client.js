import axios from 'axios'

const apiClient = axios.create({
  baseURL: '/api',
})

// Agrega el Bearer token automáticamente en cada request
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Transforma errores de la API en mensajes legibles
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const message =
      error.response?.data?.message ||
      error.response?.data?.title ||
      error.message ||
      'Error inesperado'
    return Promise.reject(new Error(message))
  }
)

export default apiClient

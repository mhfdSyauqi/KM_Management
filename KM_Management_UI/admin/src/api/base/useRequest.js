import axios from 'axios'

const useRequest = axios.create({
  baseURL: 'https://localhost:44362/api',
  withCredentials: true,
  headers: { 'Content-Type': 'application/json' }
})

export { useRequest }
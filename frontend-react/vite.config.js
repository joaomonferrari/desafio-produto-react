import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  base: '/desafio-produto-react/',
  plugins: [react()],
  server: {
    proxy: { '/api': 'http://localhost:5000' } // para desenvolvimento local
  }
})

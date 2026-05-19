import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'


const target = process.env.services__webapi__http__0   
               ?? 'http://localhost:5000'; // this is a fallback port, just inecase we run vite without Aspire.
// https://vite.dev/config/


// this is for proxxying everythime we define a backend api we add that to here so that our configuration knows what's going on.
export default defineConfig({
  plugins: [react()], // we want to make sure that this runs on HTTPS.
  server: {
    port: parseInt(process.env.PORT || '5173'),
    proxy: {
      '^/stream': {
        target: target,
        secure: false
      },
    }
  },
  optimizeDeps: {
    force: false  //  don't re-bundle deps every startup, for some reason it's slow on startup
  }
})
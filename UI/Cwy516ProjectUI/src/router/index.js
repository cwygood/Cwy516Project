import Vue from 'vue'
import Router from 'vue-router'

import Login from '@/views/login'
import Index from '@/views/index'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Login',
      component: Login
    },
    {
      path: '/login',
      component: Login
    },
    {
      path: '/index',
      component: Index
    }
  ]
})

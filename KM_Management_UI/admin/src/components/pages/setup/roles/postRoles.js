import { ref } from 'vue'
import { GetUserReferenceAsync, PostUserRoleAsync } from '@/api/reqRole.js'

const newUser = ref({
  user_name: '',
  full_name: '',
  email: '',
  job_title: '',
  role: ''
})

const error = ref({
  username: false,
  role: false
})

const userRef = ref([])
const query = ref('')
const selected = ref('')

async function GetUserReferences() {
  const result = await GetUserReferenceAsync(query.value)

  if (result.is_success) {
    userRef.value = result.user_ref
  }
}

async function HandleSave() {
  const isValid = ValidateEmpty()
  if (!isValid) return false

  const postRole = await PostUserRoleAsync(newUser.value.user_name, newUser.value.role)

  if (!postRole.is_success) {
    return 500
  }

  return true
}

function ValidateEmpty() {
  if (newUser.value.user_name === '') {
    error.value.username = true
    return false
  }

  if (newUser.value.role === '') {
    error.value.role = true
    return false
  }

  error.value.username = false
  error.value.role = false
  return true
}
function ResetInput() {
  newUser.value.user_name = ''
  newUser.value.full_name = ''
  newUser.value.email = ''
  newUser.value.job_title = ''
  newUser.value.role = ''

  query.value = ''
  selected.value = ''
  userRef.value = []

  error.value.username = false
  error.value.role = false
}

export { newUser, userRef, query, selected, error, GetUserReferences, ResetInput, HandleSave }
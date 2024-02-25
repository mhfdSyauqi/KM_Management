import { GetCategoryReferenceAsync } from '@/api/reqCategory.js'
import { ref } from 'vue'
import { PostContentAsync } from '@/api/reqContent.js'

const categoryRef = ref([])

const newArticle = ref({
  title: '',
  categoryId: '',
  description: '',
  article: '',
  addtionalLink: ''
})

const errorInput = ref({
  title: { isError: false, message: '' },
  categoryId: { isError: false, message: '' },
  description: { isError: false, message: '' },
  article: { isError: false, message: '' }
})

async function GetCategoryReference() {
  const result = await GetCategoryReferenceAsync()
  if (result.is_success) {
    categoryRef.value = result.reference.map((item) => {
      return {
        id: item.category_id,
        name: item.category_name
      }
    })
  }
}

async function HandlePublish() {
  // TODO: Add Confirmation Dialog
  const isValid = ValidateEmptyInput()
  if (!isValid) return

  const postArticle = await PostContentAsync(
    newArticle.value.title,
    newArticle.value.categoryId,
    JSON.stringify(newArticle.value.description),
    JSON.stringify(newArticle.value.article),
    newArticle.value.addtionalLink
  )

  ResetModel()

  return postArticle.is_success
  // TODO: Add Success Or Error Notification
}

function ValidateEmptyInput() {
  let valid = true
  for (const key in newArticle.value) {
    if (key !== 'addtionalLink' && newArticle.value[key] === '') {
      errorInput.value[key].isError = true
      errorInput.value[key].message = 'Required'
      valid = false
    }
  }
  return valid
}

function ResetModel() {
  newArticle.value.title = ''
  newArticle.value.categoryId = ''
  newArticle.value.description = ''
  newArticle.value.article = ''
  newArticle.value.addtionalLink = ''
}

export { categoryRef, newArticle, errorInput, GetCategoryReference, HandlePublish }